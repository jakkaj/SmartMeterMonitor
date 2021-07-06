package amber_test

import (
	"pkg/amber/pkg/amber"
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestUuth(t *testing.T) {
	err := amber.AmberAuth()
	assert.NoError(t, err)
}
