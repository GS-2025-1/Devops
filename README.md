
# 🌊 Alagamenos - Projeto DevOps - Global Solution - FIAP 2025/1

Este repositório contém o projeto desenvolvido para a disciplina **DevOps Tools & Cloud Computing**, da FIAP 2025/1.

### 👥 Integrantes do Projeto

- **Gustavo de Aguiar Lima Silva** - RM: 557707  
- **Julio Cesar Conceição Rodrigues** - RM: 557298  
- **Matheus de Freitas Silva** - RM: 552602  

### 💡 Descrição da Solução

O projeto **Alagamenos - DevOps**, desenvolvido pelo grupo **Impacto Zero** para a disciplina de **DevOps Tools & Cloud Computing**, implementa a infraestrutura base do sistema **Alaga Menos** — uma solução tecnológica voltada ao monitoramento inteligente de alagamentos em centros urbanos.

O sistema é composto por uma rede de sensores **IoT**, instalados em pontos críticos das cidades, que detectam e transmitem dados de nível de água em tempo real via protocolo **MQTT** para uma **API RESTful containerizada**. Essa API é responsável por receber, tratar e armazenar os dados em um banco de dados Oracle XE, também containerizado.

Como resultado, a infraestrutura disponibiliza informações que serão consumidas por um aplicativo **mobile**, permitindo que moradores consultem em tempo real as regiões afetadas por alagamentos, além de um painel web destinado a agentes públicos para visualização e análise histórica dos dados.

No escopo desta entrega, foi realizada a containerização completa da API e do banco de dados, com deploy automatizado em uma **VM Linux na Azure**, criação de rede personalizada entre os containers, gerenciamento via terminal e uso de boas práticas com variáveis de ambiente e persistência de dados — formando a base sólida para o funcionamento e escalabilidade do sistema **Alaga Menos**.

A aplicação contempla:

- **API em .NET 9**  
- **Banco de Dados Oracle XE** com volume persistente  
- Containers configurados com **boas práticas de segurança (não root)**  
- **Rede Docker personalizada** para comunicação interna entre API e banco  
- Deploy automatizado via **CLI da Azure**  
- Execução e gerenciamento dos containers via **terminal (sem uso de interfaces gráficas)**

---

## 🎯 Objetivo

Demonstrar a utilização de:

- Docker
- Dockerfile com boas práticas
- Variáveis de ambiente
- Persistência de dados com volumes
- Rede Docker personalizada
- Execução automatizada via CLI
- Deploy de containers em uma VM Linux na Azure

---

## ⚙️ Estrutura do Projeto

### 📦 Containers utilizados

| Container | Imagem | Versão | Configuração |
|-----------|--------|--------|--------------|
| API (.NET) | `julioccr/gs_api` | `v1.1` | Porta 5000, variável `ASPNETCORE_ENVIRONMENT` |
| Banco Oracle XE | `julioccr/oracle_gs` | `v1.0` | Porta 1521, volume `oracle_data`, variável `ORACLE_PASSWORD` |

---

### 📂 Localização dos Dockerfiles

| Serviço | Caminho do Dockerfile no repositório |
|---------|--------------------------------------|
| API (.NET) | `gs-api/Alagamenos/Dockerfile` |
| Banco Oracle XE | `gs-oracle-db/Dockerfile` |

---

### 🛠️ Como Executar o Projeto na VM Linux (Azure)

#### ✅ Pré-requisitos

- Conta na Azure  
- Azure CLI configurada  
- Acesso SSH à VM criada  
- Docker instalado na VM

---

#### 🚀 Passo a passo resumido

##### 1️⃣ Criação da VM Linux via CLI

```bash
az login
az group create -l eastus -n rg-gs
az vm create --resource-group rg-gs --name vm-gs --image Canonical:ubuntu-24_04-lts:minimal:24.04.202505020 --size Standard_B2s --admin-username admin_fiap --admin-password 'Admin_fiap@123!'
```

##### 2️⃣ Liberação de portas

```bash
az network nsg rule create --resource-group rg-gs --nsg-name vm-gsNSG --name port_5000 --protocol tcp --priority 1010 --destination-port-range 5000 --access Allow --direction Inbound
az network nsg rule create --resource-group rg-gs --nsg-name vm-gsNSG --name port_1521 --protocol tcp --priority 1020 --destination-port-range 1521 --access Allow --direction Inbound
```

##### 3️⃣ Instalação do Docker

```bash
sudo apt update
sudo apt install docker.io -y
sudo usermod -aG docker $USER
# Reconnect SSH
docker login -u <Usuario Docker>
```

##### 4️⃣ Criação de rede Docker

```bash
docker network create gs-network
```

##### 5️⃣ Pull e execução dos containers

###### Banco Oracle XE

```bash
docker pull julioccr/oracle_gs:v1.0
docker run -d --name oracle_gs --network gs-network -p 1521:1521 -e ORACLE_PASSWORD=Fiap12345 -v oracle_data:/opt/oracle/oradata julioccr/oracle_gs:v1.0
```

###### API .NET

```bash
docker pull julioccr/gs_api:v1.1
docker run -d --name gs_api --network gs-network -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Development julioccr/gs_api:v1.1
```

---

### 🌐 Como acessar a aplicação

#### Acessar a API (.NET):

- URL: `http://<IP_PUBLICO_VM>:5000`  
- Você pode acessar via navegador ou ferramentas como **Postman**.

#### Acessar o banco de dados (Oracle XE):

1. Utilize uma ferramenta como **SQL Developer**.  
2. Configuração de conexão:

| Campo | Valor |
|-------|-------|
| Usuário | SYSTEM |
| Senha | Fiap12345 |
| Hostname | <IP_PUBLICO_VM> |
| Porta | 1521 |
| Service Name | XEPDB1 |

3. Após conectar com o usuário **SYSTEM**, executar:

```sql
ALTER SESSION SET CONTAINER = XEPDB1;
```

4. Atribuir Permissões ao usuário **FIAP**:

```sql
GRANT CONNECT, RESOURCE, CREATE SESSION, CREATE TABLE, CREATE VIEW, CREATE SEQUENCE TO FIAP;
ALTER USER FIAP QUOTA UNLIMITED ON USERS;
```

5. Conectar com o usuário **FIAP**:

| Campo | Valor |
|-------|-------|
| Usuário | FIAP |
| Senha | Fiap12345 |
| Hostname | <IP_PUBLICO_VM> |
| Porta | 1521 |
| Service Name | XEPDB1 |

6. Criar tabelas, procedures e popular dados:  
Executar os scripts de **criação de tabelas**, **procedures** e **inserts** disponíveis no repositório, pasta gs-oracle-db.

---

### 📦 Tecnologias Utilizadas

- Docker
- .NET 9
- Banco de Dados Oracle XE 21.3
- Azure CLI
- Rede Docker customizada
- Volume persistente no container do banco
- API com variável de ambiente
- Execução em background

---

### 📬 Operações Demonstradas no Vídeo

- Criação da VM via CLI
- Instalação do Docker
- Criação da rede Docker
- Pull e execução dos containers
- `docker ps` → containers rodando
- Acesso à API via navegador: `http://<IP_PUBLICO_VM>:5000`
- Acesso ao banco via SQL Developer
- Criação do usuário FIAP, permissões e execução de scripts
- `docker stop` → parada dos containers
- Verificação que a API e o banco pararam de responder
- `docker rm` → remoção dos containers
- `docker ps -a` → containers removidos
- `history` → histórico completo dos comandos executados

---

### 🎥 Link para o vídeo demonstrativo

👉 https://youtu.be/f8xqTx136CI

---

### 📌 Observações

- A aplicação foi configurada para **não rodar como root** (Dockerfile da API com `USER appuser`).
- O banco de dados foi configurado com volume persistente (`oracle_data`).
- Todo o processo foi realizado via **terminal**, conforme requisitos da disciplina.

---

### 👨‍💻 Autores
Projeto Alaga Menos

Grupo Impacto Zero — Global Solution FIAP 2025
