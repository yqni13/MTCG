 DROP TABLE IF EXISTS tradings CASCADE;
 DROP TABLE IF EXISTS sessions CASCADE;
 DROP TABLE IF EXISTS cards CASCADE;
 DROP TABLE IF EXISTS decks CASCADE;
 DROP TABLE IF EXISTS users CASCADE;
 

CREATE TABLE
    IF NOT EXISTS
        users (
            u_id    uuid PRIMARY KEY,
            u_username    varchar NOT NULL UNIQUE,
            u_password    varchar NOT NULL,
            u_coins   integer default 20,
            u_elo integer default 100,
            u_bio text,
            u_image text,
            u_games integer default 0,
            u_wins integer default 0,
            u_losses integer default 0
);

CREATE TABLE
    IF NOT EXISTS
        decks (
            d_id    serial NOT NULL CONSTRAINT decks_pk PRIMARY KEY,
            d_user  uuid NOT NULL CONSTRAINT decks_users_u_id_fk REFERENCES users           
);

CREATE TABLE
    IF NOT EXISTS
        cards (
            c_id    uuid NOT NULL CONSTRAINT cards_pk PRIMARY KEY,
            c_name varchar NOT NULL,
            c_stackuser  uuid CONSTRAINT cards_users_u_id_fk REFERENCES users,
            c_damage    integer,            
            c_cardtype varchar,
            c_elementtype  varchar,
            c_fortrade boolean default false,            
            c_packagetimestamp timestamp,
            c_indeck integer CONSTRAINT cards_decks_d_id_fk REFERENCES decks
);

CREATE TABLE
    IF NOT EXISTS
        sessions (
            s_token     varchar NOT NULL,
            s_user  uuid NOT NULL CONSTRAINT sessions_users_u_id_fk REFERENCES users on delete cascade,
            s_timestamp timestamp NOT NULL
);

CREATE TABLE
    IF NOT EXISTS
        tradings (
            t_id    uuid NOT NULL CONSTRAINT tradings_pk PRIMARY KEY,
            t_user  uuid NOT NULL CONSTRAINT tradings_users_u_id_fk REFERENCES users,
            t_card  uuid NOT NULL CONSTRAINT tradings_cards_c_id_fk REFERENCES cards,
            t_mindamage int,
            t_requiredtype varchar
);



