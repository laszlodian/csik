-- Database: csikvagas

-- DROP DATABASE csikvagas;

CREATE DATABASE csikvagas
  WITH OWNER = csikvagasadmin
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       CONNECTION LIMIT = -1;
GRANT ALL ON DATABASE csikvagas TO public;
GRANT ALL ON DATABASE csikvagas TO csikvagasadmin;

COMMENT ON DATABASE csikvagas
  IS 'Labornak ldian gazdaja';

-- Table: accuracy_results

-- DROP TABLE accuracy_results;

CREATE TABLE accuracy_results
(
  pk_id serial NOT NULL,
  bias_value double precision,
  outside_bias_limit double precision,
  percent_outside_bias_limit double precision,
  master_avg double precision,
  master_sd double precision,
  CONSTRAINT accuracy_results_pkey PRIMARY KEY (pk_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE accuracy_results
  OWNER TO csikvagasadmin;


-- Table: blank_test_averages

-- DROP TABLE blank_test_averages;

CREATE TABLE blank_test_averages
(
  avg double precision,
  roll_id character varying,
  fk_blank_test_identify_id integer,
  blank_is_valid boolean,
  tube_count_in_one_roll integer,
  stddev double precision,
  cv double precision,
  date timestamp without time zone,
  lot_id character varying,
  remeasured boolean,
  invalidate boolean,
  pk_id serial NOT NULL,
  CONSTRAINT blank_test_averages_pkey PRIMARY KEY (pk_id),
  CONSTRAINT fkey_identify FOREIGN KEY (fk_blank_test_identify_id)
      REFERENCES blank_test_identify (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_averages
  OWNER TO csikvagasadmin;

-- Index: fki_fkey_identify

-- DROP INDEX fki_fkey_identify;

CREATE INDEX fki_fkey_identify
  ON blank_test_averages
  USING btree
  (fk_blank_test_identify_id);

-- Table: blank_test_environment

-- DROP TABLE blank_test_environment;

CREATE TABLE blank_test_environment
(
  user_name character varying,
  computer_name character varying,
  start_date timestamp without time zone,
  end_date timestamp without time zone,
  temperature double precision,
  remeasured boolean,
  invalidate boolean,
  pk_id serial NOT NULL,
  fk_blank_test_result_id integer,
  CONSTRAINT blank_test_environment_pkey PRIMARY KEY (pk_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_environment
  OWNER TO csikvagasadmin;


-- Table: blank_test_errors

-- DROP TABLE blank_test_errors;

CREATE TABLE blank_test_errors
(
  error character varying,
  error_text character varying,
  not_h62_error boolean,
  h62_error boolean,
  early_dribble boolean,
  device_replace boolean,
  remeasured boolean,
  invalidate boolean,
  pk_id serial NOT NULL,
  CONSTRAINT pk_id PRIMARY KEY (pk_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_errors
  OWNER TO csikvagasadmin;


-- Table: blank_test_identify

-- DROP TABLE blank_test_identify;

CREATE TABLE blank_test_identify
(
  lot_id character varying,
  roll_id character varying,
  temperature_ok boolean,
  sub_roll_id character varying,
  serial_number character varying,
  barcode character varying,
  measure_type character varying,
  remeasured boolean,
  invalidate boolean,
  pk_id serial NOT NULL,
  fk_blank_test_result_id integer,
  CONSTRAINT blank_test_identify_pkey PRIMARY KEY (pk_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_identify
  OWNER TO csikvagasadmin;

-- Index: fki_aa

-- DROP INDEX fki_aa;

CREATE INDEX fki_aa
  ON blank_test_identify
  USING btree
  (fk_blank_test_result_id);


-- Table: blank_test_result

-- DROP TABLE blank_test_result;

CREATE TABLE blank_test_result
(
  code integer,
  glu double precision,
  measure_id integer,
  serial_number integer,
  is_check_strip boolean,
  fk_blank_test_errors_id integer,
  nano_amper double precision,
  master_lot boolean,
  remeasured boolean,
  invalidate boolean,
  pk_id serial NOT NULL,
  CONSTRAINT blank_test_result_pkey PRIMARY KEY (pk_id),
  CONSTRAINT errorsid FOREIGN KEY (fk_blank_test_errors_id)
      REFERENCES blank_test_errors (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE blank_test_result
  OWNER TO csikvagasadmin;

-- Index: fki_errorsid

-- DROP INDEX fki_errorsid;

CREATE INDEX fki_errorsid
  ON blank_test_result
  USING btree
  (fk_blank_test_errors_id);

-- Table: homogenity_result

-- DROP TABLE homogenity_result;

CREATE TABLE homogenity_result
(
  homogenity_is_valid boolean,
  roll_id character varying,
  fk_blank_test_identify_id integer,
  strip_count_in_one_roll integer,
  avg double precision,
  stddev double precision,
  cv double precision,
  not_h62_error_count integer,
  h62_errors_count integer,
  date timestamp without time zone,
  lot_id character varying,
  out_of_range_strip_count integer,
  out_of_range_is_valid boolean,
  tube_count integer,
  not_h62_is_valid boolean,
  invalidate boolean,
  pk_id serial NOT NULL,
  remeasured boolean,
  CONSTRAINT homogenity_result_pkey PRIMARY KEY (pk_id),
  CONSTRAINT id FOREIGN KEY (fk_blank_test_identify_id)
      REFERENCES blank_test_identify (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE homogenity_result
  OWNER TO csikvagasadmin;

-- Index: fki_fkey_averages

-- DROP INDEX fki_fkey_averages;

CREATE INDEX fki_fkey_averages
  ON homogenity_result
  USING btree
  (fk_blank_test_identify_id);

-- Table: homogenity_test

-- DROP TABLE homogenity_test;

CREATE TABLE homogenity_test
(
  strip_ok boolean,
  fk_blank_test_result_id integer,
  roll_id character varying,
  lot_id character varying,
  invalidate boolean,
  pk_id serial NOT NULL,
  remeasured boolean,
  CONSTRAINT homogenity_test_pkey PRIMARY KEY (pk_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE homogenity_test
  OWNER TO csikvagasadmin;

-- Index: fki_fkey_blank_result

-- DROP INDEX fki_fkey_blank_result;

CREATE INDEX fki_fkey_blank_result
  ON homogenity_test
  USING btree
  (fk_blank_test_result_id);

-- Table: lot_result

-- DROP TABLE lot_result;

CREATE TABLE lot_result
(
  avg double precision,
  stddev double precision,
  cv double precision,
  fk_homogenity_result_id integer,
  avg_ok boolean,
  cv_ok boolean,
  not_h62_strip_errors integer,
  h62_strip_errors integer,
  lot_id character varying,
  lot_strip_count integer,
  lot_is_valid boolean,
  date timestamp without time zone,
  invalidate boolean,
  pk_id serial NOT NULL,
  CONSTRAINT lot_result_pkey PRIMARY KEY (pk_id),
  CONSTRAINT lot_result_fk_homogenity_result_id_fkey FOREIGN KEY (fk_homogenity_result_id)
      REFERENCES homogenity_result (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE lot_result
  OWNER TO csikvagasadmin;
-- Table: measure_type

-- DROP TABLE measure_type;

CREATE TABLE measure_type
(
  fk_blank_test_errors_id integer,
  measure_type character varying,
  invalidate boolean,
  pk_id serial NOT NULL,
  CONSTRAINT measure_type_pkey PRIMARY KEY (pk_id),
  CONSTRAINT fkey_blank_errors FOREIGN KEY (fk_blank_test_errors_id)
      REFERENCES blank_test_errors (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE measure_type
  OWNER TO csikvagasadmin;

-- Index: fki_fkey_blank_errors

-- DROP INDEX fki_fkey_blank_errors;

CREATE INDEX fki_fkey_blank_errors
  ON measure_type
  USING btree
  (fk_blank_test_errors_id);

-- Table: roll_result

-- DROP TABLE roll_result;

CREATE TABLE roll_result
(
  pk_id serial NOT NULL,
  pk_id serial NOT NULL,
  roll_id character varying,
  lot_id character varying,
  roll_avg double precision,
  roll_stddev double precision,
  avg_ok boolean,
  cv_ok boolean,
  fk_lot_result_id integer,
  roll_cv double precision,
  roll_is_valid boolean,
  roll_strip_count integer,
  roll_date timestamp without time zone,
  out_of_range_strip_count integer,
  invalidate boolean,
  CONSTRAINT roll_result_pkey PRIMARY KEY (pk_id),
  CONSTRAINT roll_result_fk_lot_result_id_fkey FOREIGN KEY (fk_lot_result_id)
      REFERENCES lot_result (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE roll_result
  OWNER TO csikvagasadmin;

