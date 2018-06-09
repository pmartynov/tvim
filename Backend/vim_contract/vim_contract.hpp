/**
 *  @file
 *  @copyright defined in eos/LICENSE.txt
 */
#include "structures.hpp"

namespace eosio {

using namespace structures;

class vim_controller : public contract {
public:
    vim_controller(account_name m_self);

    void apply(uint64_t, uint64_t action);

private:
    void init_contract();
    void create_account(const st_account &m_st_account);
    void create_post(const st_post_not_id &m_st_post);
    void transfer(const st_transfer &m_st_transfer);
    void emission(const st_hash &m_st_hash);

    void inline_emission();
    void sub_balance(account_name owner, asset value);
    void add_balance(account_name owner, asset value, account_name ram_payer);

private: //TODO auxiliary methods
    template <typename T, typename K> // TODO T - table, K - key in table
    bool find_data(const T &m_table, const K &m_key);

private:
    tables::posts_table _table_posts;
    tables::accounts_table _table_accounts;
};

}
