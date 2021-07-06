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

func AmberAuth() (err error) {
	// configure cognito srp

	user := os.Getenv("USER")
	password := os.Getenv("PASSWORD")

	if user == "" || password == "" {
		return fmt.Errorf("USER and PASSWORD env vars must be set")
	}

	csrp, err := cognitosrp.NewCognitoSRP(user, password, "ap-southeast-2_vPQVymJLn", "11naqf0mbruts1osrjsnl2ee1", nil)
	if err != nil {
		return err
	}

	// configure cognito identity provider
	cfg, err := config.LoadDefaultConfig(context.TODO(),
		config.WithRegion("ap-southeast-2"),
		config.WithCredentialsProvider(aws.AnonymousCredentials{}),
	)

	if err != nil {
		return err
	}

	svc := cip.NewFromConfig(cfg)

	// initiate auth
	resp, err := svc.InitiateAuth(context.Background(), &cip.InitiateAuthInput{
		AuthFlow:       types.AuthFlowTypeUserSrpAuth,
		ClientId:       aws.String(csrp.GetClientId()),
		AuthParameters: csrp.GetAuthParams(),
	})
	if err != nil {
		panic(err)
	}

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

		// print the tokens
		fmt.Printf("Access Token: %s\n", *resp.AuthenticationResult.AccessToken)
		fmt.Printf("ID Token: %s\n", *resp.AuthenticationResult.IdToken)
		fmt.Printf("Refresh Token: %s\n", *resp.AuthenticationResult.RefreshToken)
	}

	return nil
}
