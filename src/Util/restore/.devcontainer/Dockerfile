FROM golang:1.16.3-buster

RUN apt-get update && apt-get -y install --no-install-recommends apt-transport-https ca-certificates curl gnupg2 lsb-release zsh

RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)"

#RUN curl -Lo ./kind https://kind.sigs.k8s.io/dl/v0.10.0/kind-linux-amd64 && \
#    chmod +x ./kind && mv ./kind /bin/kind

#RUN curl -Lo ./k9s.tar.gz https://github.com/derailed/k9s/releases/download/v0.24.7/k9s_Linux_x86_64.tar.gz && \
#    mkdir k9s && tar xvzf k9s.tar.gz -C ./k9s && mv ./k9s/k9s /bin/k9s

RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | apt-key add - && \
    echo "deb https://apt.kubernetes.io/ kubernetes-xenial main" | tee -a /etc/apt/sources.list.d/kubernetes.list && \
    curl -fsSL https://download.docker.com/linux/$(lsb_release -is | tr '[:upper:]' '[:lower:]')/gpg | (OUT=$(apt-key add - 2>&1) || echo $OUT) && \
    echo "deb [arch=amd64] https://download.docker.com/linux/$(lsb_release -is | tr '[:upper:]' '[:lower:]') $(lsb_release -cs) stable" | tee /etc/apt/sources.list.d/docker.list && \
    curl -sL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor | tee /etc/apt/trusted.gpg.d/microsoft.gpg > /dev/null && \
    echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $(lsb_release -cs) main" | tee /etc/apt/sources.list.d/azure-cli.list

RUN apt-get update && apt-get install -y --no-install-recommends docker-ce-cli azure-cli unzip && \
    apt-get autoremove -y && apt-get clean -y && \
    rm -rf /var/lib/apt/lists/*

RUN GO111MODULE=on go get -v \
    golang.org/x/tools/gopls@latest \
    golang.org/x/lint/golint@latest \
    github.com/go-delve/delve/cmd/dlv@latest \
    2>&1

RUN GO111MODULE=on go get -v \
    github.com/jstemmer/go-junit-report@latest \
    github.com/axw/gocov/gocov@latest \
    github.com/AlekSi/gocov-xml \
    github.com/wadey/gocovmerge \
    2>&1

RUN curl -sSfL https://raw.githubusercontent.com/golangci/golangci-lint/master/install.sh | sh -s -- -b $(go env GOPATH)/bin v1.35.2

RUN go get github.com/spf13/cobra/cobra
RUN go get github.com/briandowns/spinner
RUN go get github.com/gorilla/mux
RUN go get gopkg.in/yaml.v2
RUN go get github.com/sirupsen/logrus

RUN wget https://dl.influxdata.com/influxdb/releases/influxdb2-2.0.6-amd64.deb
RUN dpkg -i influxdb2-2.0.6-amd64.deb