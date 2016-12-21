-- Table: lot_result_modified

-- DROP TABLE lot_result_modified;

CREATE TABLE lot_result_modified
(
  avg double precision,
  cv double precision,
  fk_lot_result_id integer,
  avg_ok boolean,
  cv_ok boolean,
  not_h62_strip_errors integer,
  h62_strip_errors integer,
  lot_id character varying,
  lot_strip_count integer,
  lot_is_valid boolean,
  date timestamp without time zone,
  invalidate boolean,
  out_of_range_strip_count integer,
  pk_id serial NOT NULL,
  CONSTRAINT lot_result_modified_pkey PRIMARY KEY (pk_id ),
  CONSTRAINT lot_result_modified_fk_homogenity_result_id_fkey FOREIGN KEY (fk_lot_result_id)
      REFERENCES lot_result (pk_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE lot_result_modified
  OWNER TO csikvagasadmin;
GRANT ALL ON TABLE lot_result_modified TO csikvagasadmin;
GRANT ALL ON TABLE lot_result_modified TO public;

-- Index: fkii_id

-- DROP INDEX fkii_id;

CREATE INDEX fkii_id
  ON lot_result_modified
  USING btree
  (fk_lot_result_id );


-- Trigger: RI_ConstraintTrigger_2031023 on lot_result_modified

-- DROP TRIGGER "RI_ConstraintTrigger_2031023" ON lot_result_modified;

CREATE CONSTRAINT TRIGGER "RI_ConstraintTrigger_2031023"
  AFTER INSERT
  ON lot_result_modified
  FOR EACH ROW
  EXECUTE PROCEDURE "RI_FKey_check_ins"();

-- Trigger: RI_ConstraintTrigger_2031024 on lot_result_modified

-- DROP TRIGGER "RI_ConstraintTrigger_2031024" ON lot_result_modified;

CREATE CONSTRAINT TRIGGER "RI_ConstraintTrigger_2031024"
  AFTER UPDATE
  ON lot_result_modified
  FOR EACH ROW
  EXECUTE PROCEDURE "RI_FKey_check_upd"();

