FROM python:2.7-stretch


RUN pip install pyserial flask requests pytz python-dotenv

WORKDIR /src

COPY ./ ./

CMD ["python", "server.py"]