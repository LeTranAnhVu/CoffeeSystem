# build stage
FROM node:14.18.1-alpine3.12 as build-stage
WORKDIR /app
COPY employee-vue-app/package*.json ./
RUN npm install
COPY employee-vue-app/ .
RUN npm run build

# production stage
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]