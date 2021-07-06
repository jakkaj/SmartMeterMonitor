package webserver

import (
	"amberdata/pkg/amber"
	"amberdata/pkg/api"
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
	app.Handle("/amber", amber.HandleAmberRequest)
	server := api.New(3004, app)

	err := server.Run()
	if err != nil {
		fmt.Printf("error: %v", err)
	}

	shutdownDuration := 5 * time.Second
	fmt.Printf("allowing %s for graceful shutdown to complete", shutdownDuration)
	<-time.After(shutdownDuration)
}
