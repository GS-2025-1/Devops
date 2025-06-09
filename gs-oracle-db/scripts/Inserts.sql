--- EXECUÇÃO ---
--- Estado
BEGIN
  inserir_estado('São Paulo');
  inserir_estado('Rio de Janeiro');
  inserir_estado('Minas Gerais');
  inserir_estado('Bahia');
  inserir_estado('Paraná');
END;
/

--- Cidade
DECLARE
  v_id_estado NUMBER;
BEGIN
  SELECT ID INTO v_id_estado FROM ESTADO WHERE NOME_ESTADO = 'São Paulo';

  inserir_cidade('São Paulo', v_id_estado);
  inserir_cidade('Campinas', v_id_estado);
  inserir_cidade('Santos', v_id_estado);
  inserir_cidade('Ribeirão Preto', v_id_estado);
  inserir_cidade('São Bernardo do Campo', v_id_estado);
END;
/

--- Bairro
DECLARE
  v_id_cidade NUMBER;
BEGIN
  SELECT ID INTO v_id_cidade FROM CIDADE WHERE NOME_CIDADE = 'São Paulo';

  inserir_bairro('Moema', v_id_cidade);
  inserir_bairro('Itaim Bibi', v_id_cidade);
  inserir_bairro('Vila Mariana', v_id_cidade);
  inserir_bairro('Pinheiros', v_id_cidade);
  inserir_bairro('Tatuapé', v_id_cidade);
END;
/

--- Rua
DECLARE
  v_bairro_id NUMBER;
BEGIN
  SELECT ID INTO v_bairro_id FROM BAIRRO WHERE NOME_BAIRRO = 'Moema';
  inserir_rua('Avenida Ibirapuera', 'Próxima ao shopping', v_bairro_id);

  SELECT ID INTO v_bairro_id FROM BAIRRO WHERE NOME_BAIRRO = 'Itaim Bibi';
  inserir_rua('Rua Joaquim Floriano', NULL, v_bairro_id);

  SELECT ID INTO v_bairro_id FROM BAIRRO WHERE NOME_BAIRRO = 'Vila Mariana';
  inserir_rua('Rua Domingos de Morais', 'Movimentada', v_bairro_id);

  SELECT ID INTO v_bairro_id FROM BAIRRO WHERE NOME_BAIRRO = 'Pinheiros';
  inserir_rua('Rua dos Pinheiros', NULL, v_bairro_id);

  SELECT ID INTO v_bairro_id FROM BAIRRO WHERE NOME_BAIRRO = 'Tatuapé';
  inserir_rua('Rua Cantagalo', 'Próxima ao metrô', v_bairro_id);
END;
/

--- Usuario
BEGIN
  inserir_usuario('João Silva', TO_DATE('10/03/1990', 'DD/MM/YYYY'), 11999998888, 'joao@gmail.com', 'senha123');
  inserir_usuario('Maria Souza', TO_DATE('25/06/1985', 'DD/MM/YYYY'), 11988887777, 'maria@gmail.com', 'senha456');
  inserir_usuario('Carlos Lima', TO_DATE('12/12/2000', 'DD/MM/YYYY'), 11977776666, 'carlos@gmail.com', 'senha789');
  inserir_usuario('Ana Paula', TO_DATE('01/01/1995', 'DD/MM/YYYY'), 11966665555, 'ana@gmail.com', 'senhaabc');
  inserir_usuario('Pedro Henrique', TO_DATE('05/05/1988', 'DD/MM/YYYY'), 11955554444, 'pedro@gmail.com', 'senhaxyz');
END;
/

--- Endereco
DECLARE
  CURSOR c_usuarios IS
    SELECT ID, EMAIL FROM USUARIO;

  v_usuario_id USUARIO.ID%TYPE;
  v_rua_id RUA.ID%TYPE;
  v_rua_nome VARCHAR2(100);
  v_numero NUMBER := 100;
  v_complemento VARCHAR2(50);
BEGIN
  FOR usuario_rec IN c_usuarios LOOP
    IF usuario_rec.EMAIL = 'joao@gmail.com' THEN
      v_rua_nome := 'Rua Cantagalo';
      v_complemento := 'Apto 101';
    ELSIF usuario_rec.EMAIL = 'maria@gmail.com' THEN
      v_rua_nome := 'Rua Domingos de Morais';
      v_complemento := 'Casa';
    ELSIF usuario_rec.EMAIL = 'carlos@gmail.com' THEN
      v_rua_nome := 'Rua Joaquim Floriano';
      v_complemento := 'Bloco B';
    ELSIF usuario_rec.EMAIL = 'ana@gmail.com' THEN
      v_rua_nome := 'Rua dos Pinheiros';
      v_complemento := 'Sala 202';
    ELSIF usuario_rec.EMAIL = 'pedro@gmail.com' THEN
      v_rua_nome := 'Avenida Ibirapuera';
      v_complemento := 'Cobertura';
    ELSE
      CONTINUE;
    END IF;

    SELECT ID INTO v_rua_id FROM RUA WHERE NOME_RUA = v_rua_nome;

    inserir_endereco(TO_CHAR(v_numero), v_complemento, v_rua_id, usuario_rec.ID);

    v_numero := v_numero + 1;
  END LOOP;
END;
/

--- Alerta
DECLARE
  CURSOR c_ruas IS
    SELECT ID, NOME_RUA FROM RUA;

  v_rua_id RUA.ID%TYPE;
  v_mensagem VARCHAR2(100);
BEGIN
  FOR rua_rec IN c_ruas LOOP
    -- Inserir primeiro alerta
    v_mensagem := 'Alerta de alagamento nível 1 na ' || rua_rec.NOME_RUA;
    inserir_alerta(v_mensagem, SYSDATE, rua_rec.ID);

    -- Inserir segundo alerta
    v_mensagem := 'Alerta de alagamento nível 2 na ' || rua_rec.NOME_RUA;
    inserir_alerta(v_mensagem, SYSDATE, rua_rec.ID);
  END LOOP;
END;
/


--- Usuario-Alerta
DECLARE
  CURSOR c_usuarios IS
    SELECT u.ID AS USUARIO_ID, r.ID AS RUA_ID
    FROM USUARIO u
    JOIN ENDERECO e ON u.ID = e.USUARIO_ID
    JOIN RUA r ON e.RUA_ID = r.ID;

  v_alerta_id ALERTA.ID%TYPE;
  v_contador NUMBER := 0;
BEGIN
  FOR usuario_rec IN c_usuarios LOOP
    -- Selecionar os dois alertas mais recentes da rua
    FOR alerta_rec IN (
      SELECT ID FROM ALERTA
      WHERE RUA_ID = usuario_rec.RUA_ID
      ORDER BY DATA_CRIACAO DESC
      FETCH FIRST 2 ROWS ONLY
    ) LOOP
      inserir_usuario_alerta(alerta_rec.ID, usuario_rec.USUARIO_ID);
    END LOOP;
  END LOOP;
END;
/

commit;

