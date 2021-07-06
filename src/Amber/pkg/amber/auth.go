package amber

import (
	"context"
	"fmt"
	"os"
	"time"

	cognitosrp "github.com/alexrudd/cognito-srp/v4"

	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/config"
	cip "github.com/aws/aws-sdk-go-v2/service/cognitoidentityprovider"
	"github.com/aws/aws-sdk-go-v2/service/cognitoidentityprovider/types"
)

func AmberAuth(refresh string) (amberTokens *AmberTokens, err error) {
	// configure cognito srp

	user := os.Getenv("USER")
	password := os.Getenv("PASSWORD")

	if user == "" || password == "" {
		return nil, fmt.Errorf("USER and PASSWORD env vars must be set")
	}

	csrp, err := cognitosrp.NewCognitoSRP(user, password, "ap-southeast-2_vPQVymJLn", "11naqf0mbruts1osrjsnl2ee1", nil)
	if err != nil {
		return nil, err
	}

	// configure cognito identity provider
	cfg, err := config.LoadDefaultConfig(context.TODO(),
		config.WithRegion("ap-southeast-2"),
		config.WithCredentialsProvider(aws.AnonymousCredentials{}),
	)

	if err != nil {
		return nil, err
	}

	svc := cip.NewFromConfig(cfg)

	if refresh != "" {

		fmt.Println("Attempt refresh")

		params := map[string]string{
			"REFRESH_TOKEN": refresh,
		}
		resp, errInternal := svc.InitiateAuth(context.Background(), &cip.InitiateAuthInput{
			AuthFlow:       types.AuthFlowTypeRefreshTokenAuth,
			ClientId:       aws.String(csrp.GetClientId()),
			AuthParameters: params,
		})

		if errInternal != nil {
			fmt.Println("Refresh failed")
			return AmberAuth("")
		}

		amberTokens := &AmberTokens{
			AccessToken: *resp.AuthenticationResult.AccessToken,
			IDToken:     *resp.AuthenticationResult.IdToken,
		}

		return amberTokens, nil
	}

	fmt.Println("Attempting user and pass")
	// initiate auth
	resp, err := svc.InitiateAuth(context.Background(), &cip.InitiateAuthInput{
		AuthFlow:       types.AuthFlowTypeUserSrpAuth,
		ClientId:       aws.String(csrp.GetClientId()),
		AuthParameters: csrp.GetAuthParams(),
	})
	if err != nil {
		return nil, err
	}

	fmt.Printf("Challenge type %v \n", resp.ChallengeName)

	// respond to password verifier challenge
	if resp.ChallengeName == types.ChallengeNameTypePasswordVerifier {
		challengeResponses, _ := csrp.PasswordVerifierChallenge(resp.ChallengeParameters, time.Now())

		resp, err := svc.RespondToAuthChallenge(context.Background(), &cip.RespondToAuthChallengeInput{
			ChallengeName:      types.ChallengeNameTypePasswordVerifier,
			ChallengeResponses: challengeResponses,
			ClientId:           aws.String(csrp.GetClientId()),
		})

		if err != nil {
			panic(err)
		}

		amberTokens := &AmberTokens{
			AccessToken:  *resp.AuthenticationResult.AccessToken,
			IDToken:      *resp.AuthenticationResult.IdToken,
			RefreshToken: *resp.AuthenticationResult.RefreshToken,
		}

		return amberTokens, nil

	}

	return nil, fmt.Errorf("Something happened that we don't know about... yet")
}

type AmberTokens struct {
	AccessToken  string
	IDToken      string
	RefreshToken string
}
