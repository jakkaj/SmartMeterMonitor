FROM python:3.7-buster


RUN pip install pyserial flask requests pytz python-dotenv tesla_powerwall aiohttp

WORKDIR /src

COPY ./ ./

CMD ["python", "server.py"]