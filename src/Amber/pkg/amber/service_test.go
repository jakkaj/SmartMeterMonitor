package amber_test

import (
	"pkg/amber/pkg/amber"
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestUuth(t *testing.T) {
	amberResult, err := amber.AmberAuth("")
	assert.NoError(t, err)

	assert.NotNil(t, amberResult)

	amber2, err := amber.AmberAuth(amberResult.RefreshToken)

	assert.NoError(t, err)

	assert.NotNil(t, amber2.AccessToken)

	_, err = amber.AmberAuth(amberResult.RefreshToken)

	assert.NoError(t, err)

	//break the token
	_, err = amber.AmberAuth(amberResult.RefreshToken + "slkjd")

	assert.NoError(t, err)

}
