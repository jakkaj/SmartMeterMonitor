FROM golang:1.16.3-alpine3.13 AS builder

# Set necessary environmet variables needed for our image
ENV GO111MODULE=on \
    CGO_ENABLED=0 \
    GOOS=linux \
    GOARCH=amd64 \
    GOARM=5


# Move to working directory /build
WORKDIR /build

# Copy and download dependency using go mod
COPY go.mod .
COPY go.sum .
RUN go mod download

# Copy the code into the container
COPY . .

# Build the application
RUN go build -o app


#####################################


FROM alpine

WORKDIR /app/

# Copy binary from build to main folder
COPY --from=builder /build/app .

# USER nonroot

# Command to run when starting the container
ENTRYPOINT ["./app"]
