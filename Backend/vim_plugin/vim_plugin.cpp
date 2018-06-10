#include <eosio/vim_plugin/vim_plugin.hpp>
#include <eosio/wallet_plugin/wallet.hpp>
#include <eosio/wallet_plugin/wallet_plugin.hpp>
#include <eosio/wallet_plugin/wallet_manager.hpp>
#include <eosio/chain_plugin/chain_plugin.hpp>
#include <eosio/chain/wast_to_wasm.hpp>
#include <eosio/utilities/key_conversion.hpp>
#include <eosio/chain/transaction.hpp>

#include <fc/io/json.hpp>


#include <vim_contract/vim_contract.wast.hpp>
#include <vim_contract/vim_contract.abi.hpp>

namespace eosio { namespace detail {
    struct vim_empty {};
}}

FC_REFLECT(eosio::detail::vim_empty, );

namespace eosio {
   static appbase::abstract_plugin& _vim_plugin = app().register_plugin<vim_plugin>();

   using namespace eosio::chain;

#define CALL(api_name, api_handle, call_name, INVOKE, http_response_code) \
{std::string("/v1/" #api_name "/" #call_name), \
    [this](string, string body, url_response_callback cb) mutable { \
        try { \
            if (body.empty()) body = "{}"; \
            INVOKE \
            cb(http_response_code, fc::json::to_string(result)); \
        } catch (...) { \
            http_plugin::handle_exception(#api_name, #call_name, body, cb); \
        } \
    } \
}


#define INVOKE_V_R(api_handle, call_name) \
    const auto& vs = fc::json::json::from_string(body).as<eosio::chain::signed_transaction>(); \
    api_handle->call_name(vs); \
    eosio::detail::vim_empty result;

struct vim_plugin_impl {
    void push_transaction(const signed_transaction& trx) { try {
       elog("1");
       chain_plugin& cp = app().get_plugin<chain_plugin>();
       elog("2");
//       elog("3");
       cp.accept_transaction( packed_transaction(trx), nullptr );
    } FC_CAPTURE_AND_RETHROW( (trx) )}


    void get_trx(const signed_transaction &m_trx) {
//        signed_transaction trx = m_trx;
        push_transaction(m_trx);
    }
};

vim_plugin::vim_plugin():my(new vim_plugin_impl()){}
vim_plugin::~vim_plugin(){}

void vim_plugin::set_program_options(options_description&, options_description& cfg) {
   cfg.add_options()
         ("option-name", bpo::value<string>()->default_value("default value"),
          "Option Description")
         ;
}

void vim_plugin::plugin_initialize(const variables_map& options) {
   my.reset(new vim_plugin_impl);
}

void vim_plugin::plugin_startup() {
    app().get_plugin<http_plugin>().add_api({
        CALL(vim, my, get_trx, INVOKE_V_R(my, get_trx), 200),
    });
}

void vim_plugin::plugin_shutdown() {

}

}
