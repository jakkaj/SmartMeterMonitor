FROM python:2.7-stretch


RUN pip install pyserial

WORKDIR /src

COPY ./ ./

RUN python server.py