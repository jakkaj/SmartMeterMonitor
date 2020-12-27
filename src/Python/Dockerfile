FROM sgtwilko/rpi-raspbian-opencv:latest

#RUN pip3 install pip3 --upgrade
RUN pip install --upgrade setuptools
RUN pip install smbus2
RUN pip install Flask
RUN pip install applicationinsights
RUN pip install paho-mqtt

COPY ./ /src
WORKDIR /src

EXPOSE 6000

CMD ["python", "run.py"]