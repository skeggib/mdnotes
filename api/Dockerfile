FROM node
ENV NODE_ENV production
COPY package.json package-lock.json /app/
WORKDIR /app
RUN npm install
RUN npm install -g typescript
COPY tsconfig.json /app/
COPY src /app/src
RUN tsc
COPY config /app/config
CMD ["node", "dist/index.js"]