
 DROP TABLE IF EXISTS tradings CASCADE;
 DROP TABLE IF EXISTS own_cards CASCADE;
 DROP TABLE IF EXISTS sessions CASCADE;
 DROP TABLE IF EXISTS decks CASCADE;
 DROP TABLE IF EXISTS cards CASCADE;
 DROP TABLE IF EXISTS users CASCADE;


CREATE TABLE
    IF NOT EXISTS
        users (
            u_id    SERIAL PRIMARY KEY,
            u_username    varchar NOT NULL UNIQUE,
            u_password    varchar NOT NULL,
            u_coins   integer default 20,
            u_deck SERIAL,
            u_elo integer default 0
);

CREATE TABLE
    IF NOT EXISTS
        cards (
            c_id    SERIAL NOT NULL CONSTRAINT cards_pk PRIMARY KEY,
            c_name varchar(50) NOT NULL,
            c_element_type  integer,
            c_card_type integer,
            c_damage    integer,
            c_in_deck   boolean default false,
            c_for_trade boolean default false,
            c_user  INTEGER CONSTRAINT cards_users_u_id_fk REFERENCES users
);

CREATE TABLE
    IF NOT EXISTS
        decks (
            d_id    SERIAL NOT NULL CONSTRAINT decks_pk PRIMARY KEY,
            d_user  INTEGER NOT NULL CONSTRAINT decks_users_u_id_fk REFERENCES users,
            d_name  char(36) default 'deck_nameless'
);

CREATE TABLE
    IF NOT EXISTS
        sessions (
            s_token     varchar NOT NULL,
            s_user  INTEGER NOT NULL CONSTRAINT sessions_users_u_id_fk REFERENCES users on delete cascade,
            s_timestamp timestamp NOT NULL
);

CREATE TABLE
    IF NOT EXISTS
        own_cards (
            d_id    INTEGER NOT NULL CONSTRAINT own_cards_decks_d_id_fk REFERENCES decks,
            c_id    INTEGER NOT NULL CONSTRAINT own_cards_cards_c_id_fk REFERENCES cards on delete cascade,
            CONSTRAINT own_cards_pk UNIQUE (d_id, c_id)
);

CREATE TABLE
    IF NOT EXISTS
        tradings (
            t_id    SERIAL NOT NULL CONSTRAINT tradings_pk PRIMARY KEY,
            t_user  INTEGER NOT NULL CONSTRAINT tradings_users_u_id_fk REFERENCES users,
            t_card  INTEGER NOT NULL CONSTRAINT tradings_cards_c_id_fk REFERENCES cards
);



