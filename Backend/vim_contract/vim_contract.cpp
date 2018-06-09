#include "vim_contract.hpp"
#include <eosiolib/transaction.hpp>

/**
*  The apply() methods must have C calling convention so that the blockchain can lookup and
*  call these methods.
*/
extern "C" {

   /// The apply method implements the dispatch of events to this contract
   void apply( uint64_t receiver, uint64_t code, uint64_t action ) {
      eosio::vim_controller(receiver).apply(code, action);
   }

} // extern "C"

using namespace eosio;

vim_controller::vim_controller(account_name m_self)
    : contract(m_self)
    , _table_posts(get_self(), get_self())
    , _table_accounts(get_self(), get_self())
{}

void vim_controller::apply(uint64_t /*code*/, uint64_t action) {
    switch (action) {
    case N(init):
        init_contract();
        break;
    case N(appendacc):
        create_account(unpack_action_data<st_account>());
        break;
    case N(transfer):
        transfer(unpack_action_data<st_transfer>());
        break;
    case N(emission):
        emission(unpack_action_data<st_hash>());
        break;
    case N(createpost):
        create_post(unpack_action_data<st_post_not_id>());
        break;
    default:
        eosio_assert(false, "Incorrectly specified action");
        break;
    }
}

void vim_controller::init_contract() {
    create_account(st_account(get_self()));
    inline_emission();
}

void vim_controller::create_account(const st_account &m_st_account) {
    if (find_data(_table_accounts, m_st_account.account))
        eosio_assert(false, "Account exists");
    tables::account_table table(get_self(), m_st_account.account);
    table.emplace(get_self(), [&](auto &item) {
        item.balance = vim_token;
    });

    _table_accounts.emplace(get_self(), [&](auto &item) {
        item.account = m_st_account.account;
    });
}

void vim_controller::create_post(const st_post_not_id &m_st_post_not_id) {
    _table_posts.emplace(get_self(), [&](auto &item) {
        item = st_post(m_st_post_not_id, _table_posts.available_primary_key());
    });
}

void vim_controller::transfer(const st_transfer &m_st_transfer) {

}

void vim_controller::emission(const st_hash &m_st_hash) {


    inline_emission();
}

void vim_controller::inline_emission() {
    transaction trx;
    action trx_action( permission_level(get_self(), N(active)), get_self(), N(emission), st_hash(now()));
    trx.actions.emplace_back(trx_action);
    trx.delay_sec = 5;
    trx.send(now(), get_self());
}

void vim_controller::sub_balance(account_name owner, asset value) {

}

void vim_controller::add_balance(account_name owner, asset value, account_name ram_payer) {

}

template<typename T, typename K>
bool vim_controller::find_data(const T &m_table, const K &m_key) {
    auto iterator = m_table.find(m_key);
    if ( iterator != m_table.end() )
        return true;

    return false;
}
