package amber

import (
	"context"
	"encoding/json"
	"net/http"
)

func HandleAmberRequest(ctx context.Context, w http.ResponseWriter, r *http.Request) error {

	s := NewService()

	res, err := s.Get()

	if err != nil {
		return err
	}

	return Respond(ctx, w, res, http.StatusOK)
}

// Respond converts a Go value to JSON and sends it to the client.
func Respond(ctx context.Context, w http.ResponseWriter, data interface{}, statusCode int) error {
	// If there is nothing to marshal then set status code and return.
	if statusCode == http.StatusNoContent {
		w.WriteHeader(statusCode)
		return nil
	}

	// Convert the response value to JSON.
	jsonData, err := json.Marshal(data)
	if err != nil {
		return err
	}

	// Set the content type and headers once we know marshalling has succeeded.
	w.Header().Set("Content-Type", "application/json")

	// Write the status code to the response.
	w.WriteHeader(statusCode)

	// Send the result back to the client.
	if _, err := w.Write(jsonData); err != nil {
		return err
	}

	return nil
}
