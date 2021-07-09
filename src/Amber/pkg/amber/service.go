package amber

import (
	"bytes"
	"fmt"
	"io"
	"net/http"
	"sync"
)

var baseUrl = "https://backend.amberelectric.com.au/graphql"

type Service struct {
	RefreshToken string
	AccessToken  string
}

type Response struct {
	LivePrice string
	Usage     string
}

var once sync.Once

var (
	instService *Service
)

func NewService() (service *Service) {
	once.Do(func() {

		instService = &Service{}

	})
	return instService

}

func (s *Service) Get() (res *Response, err error) {

	err = s.Authenticate()

	if err != nil {
		return
	}

	res = &Response{}

	chanLivePrice := make(chan string)
	chanUsage := make(chan string)

	go func() {
		res, errInternal := s.request(queryLivePrice)

		if errInternal != nil {
			chanLivePrice <- ""
		}

		chanLivePrice <- res
	}()

	go func() {
		res, errInternal := s.request(queryUsage)

		if errInternal != nil {
			chanUsage <- ""
		}

		chanUsage <- res
	}()

	res.LivePrice = <-chanLivePrice
	res.Usage = <-chanUsage

	return
}

func (s *Service) request(query string) (result string, err error) {
	req, err := http.NewRequest("POST", baseUrl, bytes.NewBuffer([]byte(query)))
	if err != nil {
		return "", err
	}

	req.Header.Set("authorization", fmt.Sprintf("Bearer %v", s.AccessToken))

	if err != nil {
		return "", err
	}

	client := &http.Client{}
	res, err := client.Do(req)

	if err != nil {
		return "", err
	}

	defer res.Body.Close()

	// Read the response body
	buf := new(bytes.Buffer)
	_, err = io.Copy(buf, res.Body)
	if err != nil {
		return "", err
	}

	return buf.String(), nil
}

func (s *Service) Authenticate() (err error) {

	if s.AccessToken != "" {
		//validate expiry of this token
		if ValidateExp(s.AccessToken) {
			fmt.Println("Access token still good")
			return nil
		}
	}

	login, err := AmberAuth(s.RefreshToken)

	if err != nil {
		return err
	}

	if login.RefreshToken != "" {
		s.RefreshToken = login.RefreshToken
	}

	if login.AccessToken != "" {
		s.AccessToken = login.IDToken
	}

	return

}
