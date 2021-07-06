package api

import (
	"context"
	"fmt"
	"net/http"

	"time"
)

// Server is a struct used for http.Server and App configurations.
type Server struct {
	server *http.Server
	app    *App
}

// New creates an Server value that handle a set of routes for the application.
func New(port int, app *App) *Server {
	return &Server{
		server: &http.Server{
			Addr:    fmt.Sprintf(":%d", port),
			Handler: app,
		},
		app: app,
	}
}

// Run starts the server and handles shutdowns
func (s *Server) Run() error {
	// Make a channel to listen for errors coming from the listener. Use a
	// buffered channel so the goroutine can exit if we don't collect this error.
	serverErrors := make(chan error, 1)

	// Start the service listening for requests.
	go func() {
		fmt.Printf("main: API listening on %s", s.server.Addr)
		serverErrors <- s.server.ListenAndServe()
	}()

	// =========================================================================
	// Shutdown

	// Blocking main and waiting for shutdown.
	select {
	case err := <-serverErrors:
		return fmt.Errorf("server error: %v", err)

	case sig := <-s.app.shutdown:
		fmt.Printf("main: %v: Start shutdown", sig)

		// Give outstanding requests a deadline for completion.
		ctx, cancel := context.WithTimeout(context.Background(), time.Second*5)
		defer cancel()

		// Asking listener to shutdown and shed load.
		if err := s.server.Shutdown(ctx); err != nil {
			if er := s.server.Close(); er != nil {
				return fmt.Errorf("error while forcusfully closung server: %v", er)
			}
			return fmt.Errorf("could not stop server gracefully: %v", err)
		}

		fmt.Printf("main: %v: Completed shutdown", sig)
	}

	return nil
}
