package amber_test

import (
	"clipsaldata/pkg/amber"
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestService(t *testing.T) {
	s := amber.NewService()

	err := s.Authenticate()

	assert.NoError(t, err)

	err = s.Authenticate()

	assert.NoError(t, err)

	res, err := s.Get()

	assert.NoError(t, err)

	assert.NotEqual(t, "", res.LivePrice)

	// err = ioutil.WriteFile("../../examples/livepriceresult.json", []byte(res.LivePrice), 0644)
	// assert.NoError(t, err)
	// err = ioutil.WriteFile("../../examples/usageresult.json", []byte(res.LivePrice), 0644)
	// assert.NoError(t, err)

}

func TestAuth(t *testing.T) {
	amberResult, err := amber.AmberAuth("")
	assert.NoError(t, err)

	assert.NotNil(t, amberResult)

	assert.True(t, amber.ValidateExp(amberResult.AccessToken))

	amber2, err := amber.AmberAuth(amberResult.RefreshToken)

	assert.NoError(t, err)

	assert.NotNil(t, amber2.AccessToken)

	_, err = amber.AmberAuth(amberResult.RefreshToken)

	assert.NoError(t, err)

	//break the token
	_, err = amber.AmberAuth(amberResult.RefreshToken + "slkjd")

	assert.NoError(t, err)

}
