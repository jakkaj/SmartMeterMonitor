prepare-checkin: lint
	go mod tidy

lint:
	golangci-lint run --timeout=20m

test:
	go-acc -o cover.out ./...
	go tool cover -html=cover.out -o cover.html

test-watcher:
	@echo "Running test watcher"
	bash ./.devcontainer/test_watcher.sh

build:
	go build -o clipsalserver

run: build
	./clipsalserver

