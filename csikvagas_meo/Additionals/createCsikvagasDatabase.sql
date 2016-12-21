-- Database: csikvagas

-- DROP DATABASE csikvagas;

CREATE DATABASE csikvagas
  WITH OWNER = ldian
       ENCODING = 'WIN1252'
       TABLESPACE = pg_default
       LC_COLLATE = 'English_United Kingdom.1252'
       LC_CTYPE = 'English_United Kingdom.1252'
       CONNECTION LIMIT = -1;
GRANT ALL ON DATABASE csikvagas TO public;
GRANT ALL ON DATABASE csikvagas TO ldian;

-- Table: blank_test_errors

-- DROP TABLE blank_test_errors;

CREATE TABLE blank_test_errors
(
  pk_id serial NOT NULL,
  error character varying,
  CONSTRAINT blank_test_errors_pkey PRIMARY KEY (pk_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_errors
  OWNER TO ud2_admin;

  -- Table: blank_test_environment

-- DROP TABLE blank_test_environment;

CREATE TABLE blank_test_environment
(
  pk_id serial NOT NULL,
  user_name character varying,
  computer_name character varying,
  start_date timestamp without time zone,
  end_date timestamp without time zone,
  temperature double precision,
  fk_blank_test_result_id integer,
  CONSTRAINT blank_test_environment_pkey PRIMARY KEY (pk_id),
  CONSTRAINT fkey_blank_test_result FOREIGN KEY (fk_blank_test_result_id)
      REFERENCES blank_test_result (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_environment
  OWNER TO ud2_admin;

-- Index: fki_fkey_blank_test_result

-- DROP INDEX fki_fkey_blank_test_result;

CREATE INDEX fki_fkey_blank_test_result
  ON blank_test_environment
  USING btree
  (fk_blank_test_result_id);

-- Table: blank_test_averages

-- DROP TABLE blank_test_averages;

CREATE TABLE blank_test_averages
(
  pk_id serial NOT NULL,
  avg double precision,
  roll_id character varying,
  fk_blank_test_identify_id integer,
  valid boolean,
  CONSTRAINT blank_test_averages_pkey PRIMARY KEY (pk_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_averages
  OWNER TO ud2_admin;

  -- Table: blank_test_identify

-- DROP TABLE blank_test_identify;

CREATE TABLE blank_test_identify
(
  pk_id serial NOT NULL,
  fk_blank_test_result_id integer,
  lot_id character varying,
  roll_id character varying,
  temperature_ok boolean,
  invalidate_date timestamp without time zone,
  sub_roll_id character varying,
  CONSTRAINT blank_test_identify_pkey PRIMARY KEY (pk_id),
  CONSTRAINT fk_blank_test_result FOREIGN KEY (fk_blank_test_result_id)
      REFERENCES blank_test_result (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_identify
  OWNER TO ud2_admin;

-- Index: fki_blank_test_result

-- DROP INDEX fki_blank_test_result;

CREATE INDEX fki_blank_test_result
  ON blank_test_identify
  USING btree
  (fk_blank_test_result_id);

-- Table: blank_test_result

-- DROP TABLE blank_test_result;

CREATE TABLE blank_test_result
(
  pk_id integer NOT NULL DEFAULT nextval('blanc_test_result_pk_id_seq'::regclass),
  code integer,
  glu double precision,
  wrong_step character varying,
  measure_id integer,
  serial_number integer,
  is_check_strip boolean,
  fk_blank_test_errors_id integer,
  CONSTRAINT blanc_test_result_pkey PRIMARY KEY (pk_id),
  CONSTRAINT blank_test_result_fk_blank_test_errors_id_fkey FOREIGN KEY (fk_blank_test_errors_id)
      REFERENCES blank_test_errors (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_result
  OWNER TO ud2_admin;

  