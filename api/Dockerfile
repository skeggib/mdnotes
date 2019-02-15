FROM node
COPY package.json package-lock.json /app/
WORKDIR /app
RUN npm install
RUN npm install -g typescript
COPY tsconfig.json /app/
COPY config /app/config
COPY src /app/src
RUN tsc
WORKDIR /app/dist
CMD ["node", "src/index.js"]