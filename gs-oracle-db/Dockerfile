FROM gvenzl/oracle-xe:21.3.0

ENV ORACLE_PASSWORD=Fiap12345

# Copia apenas o script de criação do usuário
COPY ./scripts/CriarUsuario.sql /docker-entrypoint-initdb.d/
