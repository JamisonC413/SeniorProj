FROM node:17-alpine

WORKDIR /frontend

# Install dependencies
COPY package*.json .
RUN npm install

# Copy local files over to container
COPY . .

# Start up web app
CMD ["npm", "start"]