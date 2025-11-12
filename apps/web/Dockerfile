FROM node:20-alpine AS build
ARG VITE_API_BASE_URL
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}
WORKDIR /app

# Install dependencies (use npm ci for reproducible builds)
COPY package.json package-lock.json ./
RUN npm ci

# Build the production bundle
COPY . .
RUN npm run build

FROM nginx:1.27-alpine AS runtime
ARG VITE_API_BASE_URL
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}

# Copy custom nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built assets
COPY --from=build /app/dist /usr/share/nginx/html

ENV NODE_ENV=production

EXPOSE 80

# Use the default nginx startup command
CMD ["nginx", "-g", "daemon off;"]

