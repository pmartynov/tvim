/**
 *  @file
 *  @copyright defined in eos/LICENSE.txt
 */
#include <eosiolib/eosio.hpp>
#include <eosiolib/asset.hpp>

#define vim_releace_tokens asset(200000000 0000, string_to_symbol(4, "VIM")) // 200 000 000.0000 VIM
#define vim_token asset(0, string_to_symbol(4, "VIM"))

namespace eosio {
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

    uint64_t primary_key()const {
        return id;
    }

    EOSLIB_SERIALIZE( st_post, (id)(creator)(url_photo)(hash_photo) )
};


struct st_account
{
    st_account() = default;

    account_name account;

    account_name primary_key()const {
        return account;
    }

    EOSLIB_SERIALIZE( st_account, (account) )
};

struct st_account_balance
{
    st_account_balance() = default;

    asset balance;

    uint64_t primary_key()const {
        return balance.symbol.name();
    }

    EOSLIB_SERIALIZE( st_account_balance, (balance) )
};

};

namespace tables {
using posts_table = multi_index<N(posts), structures::st_post>;
using account_table = multi_index<N(account), structures::st_account_balance>;
using accounts_table = multi_index<N(accounts), structures::st_account>;
};

}
