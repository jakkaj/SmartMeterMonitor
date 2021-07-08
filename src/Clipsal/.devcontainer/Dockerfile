FROM golang:1.16.3-buster

RUN apt-get clean && \
    apt-get update && \
    apt-get -y install --no-install-recommends \
      apt-transport-https \
      ca-certificates \
      curl \
      dnsutils \
      gnupg2 \
      inotify-tools \
      less \
      locales \
      lsb-release \
      nano \
      net-tools \      
      zsh \
      && \
    apt-get autoremove -y && \
    apt-get clean -y && \
    rm -rf /var/lib/apt/lists/* && \
    localedef -i en_AU -c -f UTF-8 -A /usr/share/locale/locale.alias en_AU.UTF-8

ENV LANG en_AU.UTF-8

RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" && \
    sed -i -E -e 's/^plugins=.*/plugins=(git golang helm kubectl)/' /root/.zshrc


    
#RUN apt-get update && apt-get install -y --no-install-recommends docker-ce-cli && \
#    apt-get autoremove -y && apt-get clean -y && \
#    rm -rf /var/lib/apt/lists/*

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
    gopkg.in/yaml.v2 \
    github.com/cweill/gotests/gotests \
    github.com/ory/go-acc \    
    2>&1

RUN go get -u -v \
        github.com/rakyll/gotest \
        github.com/ramya-rao-a/go-outline

RUN curl -sSfL https://raw.githubusercontent.com/golangci/golangci-lint/master/install.sh | sh -s -- -b $(go env GOPATH)/bin v1.35.2

RUN go get github.com/spf13/cobra/cobra

RUN SNIPPET="export PROMPT_COMMAND='history -a' && export HISTFILE=/commandhistory/.bash_history" \
    && echo $SNIPPET >> "/root/.bashrc"


