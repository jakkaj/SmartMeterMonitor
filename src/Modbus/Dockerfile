FROM python:2.7-stretch


RUN pip install pyserial flask

WORKDIR /src

COPY ./ ./

CMD ["python", "server.py"]