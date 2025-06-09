--- INSERÇÃO ---

--- Estado
CREATE OR REPLACE PROCEDURE inserir_estado(
    p_nome_estado IN VARCHAR2
) AS
BEGIN
  INSERT INTO ESTADO (NOME_ESTADO) 
  VALUES (p_nome_estado);
END;
/

--- Cidade
CREATE OR REPLACE PROCEDURE inserir_cidade(
  p_nome_cidade IN VARCHAR2,
  p_estado_id IN NUMBER
) AS
BEGIN
  INSERT INTO CIDADE (NOME_CIDADE, ESTADO_ID)
  VALUES (p_nome_cidade, p_estado_id);
END;
/

--- Bairro
CREATE OR REPLACE PROCEDURE inserir_bairro(
  p_nome_bairro IN VARCHAR2,
  p_cidade_id IN NUMBER
) AS
BEGIN
  INSERT INTO BAIRRO (NOME_BAIRRO, CIDADE_ID)
  VALUES (p_nome_bairro, p_cidade_id);
END;
/

--- Rua
CREATE OR REPLACE PROCEDURE inserir_rua(
  p_nome_rua IN VARCHAR2,
  p_observacao IN VARCHAR2,
  p_bairro_id IN NUMBER
) AS
BEGIN
  INSERT INTO RUA (NOME_RUA, OBSERVACAO, BAIRRO_ID)
  VALUES (p_nome_rua, p_observacao, p_bairro_id);
END;
/

-- Usuario
CREATE OR REPLACE PROCEDURE inserir_usuario(
  p_nome IN VARCHAR2,
  p_data_nascimento IN DATE,
  p_telefone IN NUMBER,
  p_email IN VARCHAR2,
  p_senha IN VARCHAR2
) AS
BEGIN
  INSERT INTO USUARIO (NOME, DATA_NASCIMENTO, TELEFONE, EMAIL, SENHA)
  VALUES (p_nome, p_data_nascimento, p_telefone, p_email, p_senha);
END;
/
 
--- Endereco
CREATE OR REPLACE PROCEDURE inserir_endereco(
  p_numero_endereco IN VARCHAR2,
  p_complemento IN VARCHAR2,
  p_rua_id IN NUMBER,
  p_usuario_id IN NUMBER
) AS
BEGIN
  INSERT INTO ENDERECO (NUMERO_ENDERECO, COMPLEMENTO, RUA_ID, USUARIO_ID)
  VALUES (p_numero_endereco, p_complemento, p_rua_id, p_usuario_id);
END;
/

--- Alerta
CREATE OR REPLACE PROCEDURE inserir_alerta(
  p_mensagem IN VARCHAR2,
  p_data_criacao IN DATE,
  p_rua_id IN NUMBER
) AS
BEGIN
  INSERT INTO ALERTA (MENSAGEM, DATA_CRIACAO, RUA_ID)
  VALUES (p_mensagem, p_data_criacao, p_rua_id);
END;
/

--- Usuario-Alerta
CREATE OR REPLACE PROCEDURE inserir_usuario_alerta(
  p_alerta_id IN NUMBER,
  p_usuario_id IN NUMBER
) AS
BEGIN
  INSERT INTO USUARIO_ALERTA (ALERTA_ID, USUARIO_ID)
  VALUES (p_alerta_id, p_usuario_id);
END;
/

--- ATUALIZAÇÃO ---

--- Estado
CREATE OR REPLACE PROCEDURE atualizar_estado(
    p_id IN NUMBER, 
    p_nome_estado IN VARCHAR2
) AS
BEGIN
  UPDATE ESTADO 
  SET NOME_ESTADO = p_nome_estado 
  WHERE ID = p_id;
END;
/

--- Cidade
CREATE OR REPLACE PROCEDURE atualizar_cidade(
  p_id IN NUMBER,
  p_nome_cidade IN VARCHAR2,
  p_estado_id IN NUMBER
) AS
BEGIN
  UPDATE CIDADE
  SET NOME_CIDADE = p_nome_cidade,
      ESTADO_ID = p_estado_id
  WHERE ID = p_id;
END;
/

--- Bairro
CREATE OR REPLACE PROCEDURE atualizar_bairro(
  p_id IN NUMBER,
  p_nome_bairro IN VARCHAR2,
  p_cidade_id IN NUMBER
) AS
BEGIN
  UPDATE BAIRRO
  SET NOME_BAIRRO = p_nome_bairro,
      CIDADE_ID = p_cidade_id
  WHERE ID = p_id;
END;
/

--- Rua
CREATE OR REPLACE PROCEDURE atualizar_rua(
  p_id IN NUMBER,
  p_nome_rua IN VARCHAR2,
  p_observacao IN VARCHAR2,
  p_bairro_id IN NUMBER
) AS
BEGIN
  UPDATE RUA
  SET NOME_RUA = p_nome_rua,
      OBSERVACAO = p_observacao,
      BAIRRO_ID = p_bairro_id
  WHERE ID = p_id;
END;
/

--- Usuario
CREATE OR REPLACE PROCEDURE atualizar_usuario(
  p_id IN NUMBER,
  p_nome IN VARCHAR2,
  p_data_nascimento IN DATE,
  p_telefone IN NUMBER,
  p_email IN VARCHAR2,
  p_senha IN VARCHAR2
) AS
BEGIN
  UPDATE USUARIO
  SET NOME = p_nome,
      DATA_NASCIMENTO = p_data_nascimento,
      TELEFONE = p_telefone,
      EMAIL = p_email,
      SENHA = p_senha
  WHERE ID = p_id;
END;
/

--- Endereco
CREATE OR REPLACE PROCEDURE atualizar_endereco(
  p_id IN NUMBER,
  p_numero_endereco IN VARCHAR2,
  p_complemento IN VARCHAR2,
  p_rua_id IN NUMBER,
  p_usuario_id IN NUMBER
) AS
BEGIN
  UPDATE ENDERECO
  SET NUMERO_ENDERECO = p_numero_endereco,
      COMPLEMENTO = p_complemento,
      RUA_ID = p_rua_id,
      USUARIO_ID = p_usuario_id
  WHERE ID = p_id;
END;
/

--- Alerta
CREATE OR REPLACE PROCEDURE atualizar_alerta(
  p_id IN NUMBER,
  p_mensagem IN VARCHAR2,
  p_data_criacao IN DATE,
  p_rua_id IN NUMBER
) AS
BEGIN
  UPDATE ALERTA
  SET MENSAGEM = p_mensagem,
      DATA_CRIACAO = p_data_criacao,
      RUA_ID = p_rua_id
  WHERE ID = p_id;
END;
/

--- Usuario-Alerta
--- Como é uma tabela de associação, não é uma boa prática criar uma atualização de alerta, então não farei.

--- EXCLUSÃO ---

--- Estado
CREATE OR REPLACE PROCEDURE excluir_estado(p_id IN NUMBER) AS
BEGIN
  DELETE FROM ESTADO WHERE ID = p_id;
END;
/

--- Cidade
CREATE OR REPLACE PROCEDURE excluir_cidade(p_id IN NUMBER) AS
BEGIN
  DELETE FROM CIDADE WHERE ID = p_id;
END;
/

--- Bairro
CREATE OR REPLACE PROCEDURE excluir_bairro(p_id IN NUMBER) AS
BEGIN
  DELETE FROM BAIRRO WHERE ID = p_id;
END;
/

--- Rua
CREATE OR REPLACE PROCEDURE excluir_rua(p_id IN NUMBER) AS
BEGIN
  DELETE FROM RUA WHERE ID = p_id;
END;
/

--- Usuario
CREATE OR REPLACE PROCEDURE excluir_usuario(p_id IN NUMBER) AS
BEGIN
  DELETE FROM USUARIO WHERE ID = p_id;
END;
/

--- Endereco
CREATE OR REPLACE PROCEDURE excluir_endereco(p_id IN NUMBER) AS
BEGIN
  DELETE FROM ENDERECO WHERE ID = p_id;
END;
/

--- Alerta
CREATE OR REPLACE PROCEDURE excluir_alerta(p_id IN NUMBER) AS
BEGIN
  DELETE FROM ALERTA WHERE ID = p_id;
END;
/

--- Usuario-Alerta
CREATE OR REPLACE PROCEDURE excluir_usuario_alerta(
  p_alerta_id IN NUMBER,
  p_usuario_id IN NUMBER
) AS
BEGIN
  DELETE FROM USUARIO_ALERTA
  WHERE ALERTA_ID = p_alerta_id AND USUARIO_ID = p_usuario_id;
END;
/

commit;




