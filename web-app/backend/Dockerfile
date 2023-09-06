FROM python:3

# Get updates
# RUN apt-get update

WORKDIR /backend

COPY requirements.txt .

RUN pip install -r requirements.txt

COPY . .

# Run instructions for this container in Docker Compose file