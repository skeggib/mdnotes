FROM postgres:11
COPY init.sql /docker-entrypoint-initdb.d/10-init.sql
ENV POSTGRES_DB notes
ENV POSTGRES_USER user
ENV POSTGRES_PASSWORD pass
