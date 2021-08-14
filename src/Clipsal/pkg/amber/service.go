package amber

import (
	"bytes"
	"fmt"
	"io"
	"net/http"
	"sync"
)

var baseUrl = "https://j6we4yx2qf.execute-api.ap-southeast-2.amazonaws.com/prod/v1/sites/523/usage?"

var instUrl = "https://j6we4yx2qf.execute-api.ap-southeast-2.amazonaws.com/prod/v1/sites/523/live_data_summary"

type Service struct {
	RefreshToken string
	AccessToken  string
}

type Response struct {
	Usage string
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

func (s *Service) GetInst() (res *Response, err error) {

	err = s.Authenticate()

	if err != nil {
		return
	}

	res = &Response{}

	chanLivePrice := make(chan string)
	//chanUsage := make(chan string)

	go func() {
		res, errInternal := s.request(instUrl, "")

		if errInternal != nil {
			chanLivePrice <- ""
		}

		chanLivePrice <- res
	}()

	res.Usage = <-chanLivePrice

	return
}

func (s *Service) Get(querystring string) (res *Response, err error) {

	err = s.Authenticate()

	if err != nil {
		return
	}

	res = &Response{}

	chanLivePrice := make(chan string)
	//chanUsage := make(chan string)

	go func() {
		res, errInternal := s.request(baseUrl, querystring)

		if errInternal != nil {
			chanLivePrice <- ""
		}

		chanLivePrice <- res
	}()

	res.Usage = <-chanLivePrice

	return
}

func (s *Service) request(url string, querystring string) (result string, err error) {

	reqUrl := url + querystring

	fmt.Printf("\nGetting: %v\n", reqUrl)
	req, err := http.NewRequest("GET", reqUrl, nil)
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
