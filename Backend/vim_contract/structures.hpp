/**
 *  @file
 *  @copyright defined in eos/LICENSE.txt
 */
#include <eosiolib/eosio.hpp>
#include <eosiolib/asset.hpp>

namespace eosio {

namespace tokens {
//using vim_token = asset(0, string_to_symbol(4, 'VIM'));
};

namespace structures {

struct st_transfer {
    st_transfer() = default;
    st_transfer(const account_name &m_from, const account_name &m_to, const asset &m_amount,
                const std::string &m_url_photo, const std::string &m_hash_photo)
        : from(m_from), to(m_to), amount(m_amount), url_photo(m_url_photo), hash_photo(m_hash_photo)
    {}

    account_name from;
    account_name to;
    asset amount;
    std::string url_photo;
    std::string hash_photo;

    EOSLIB_SERIALIZE( st_transfer, (from)(to)(amount)(url_photo)(hash_photo) )
};

struct st_hash {
    st_hash() = default;
    uint32_t hash;

    EOSLIB_SERIALIZE( st_hash, (hash) )
};

struct st_post {
    st_post() = default;

    uint64_t id;
    account_name creator;
    std::string url_photo;
    std::string hash_photo;

    EOSLIB_SERIALIZE( st_post, (id)(creator)(url_photo)(hash_photo) )
};


struct st_account
{
    st_account() = default;

    account_name account;
    EOSLIB_SERIALIZE( st_account, (account) )
};

struct st_account_balance : public st_account
{
    st_account_balance() = default;

    asset balance;
    EOSLIB_SERIALIZE( st_account_balance, (account)(balance) )
};

};
}
