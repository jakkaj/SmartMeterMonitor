package webserver

import (
	"clipsaldata/pkg/amber"
	"clipsaldata/pkg/api"
	"fmt"
	"os"
	"os/signal"
	"syscall"
	"time"
)

func Start() {

	shutdown := make(chan os.Signal, 1)
	signal.Notify(shutdown, syscall.SIGINT, syscall.SIGTERM)
	app := api.NewApp(shutdown)
	app.Handle("/clipsal", amber.HandleAmberRequest)
	app.Handle("/instant", amber.HandleInstantRequest)
	server := api.New(3005, app)

	err := server.Run()
	if err != nil {
		fmt.Printf("error: %v", err)
	}

	shutdownDuration := 5 * time.Second
	fmt.Printf("allowing %s for graceful shutdown to complete", shutdownDuration)
	<-time.After(shutdownDuration)
}
