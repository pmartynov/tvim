#include <eosio/vim_plugin/vim_plugin.hpp>

#include <eosio/chain_plugin/chain_plugin.hpp>
#include <eosio/chain/wast_to_wasm.hpp>
#include <eosio/utilities/key_conversion.hpp>

#include <fc/io/json.hpp>

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

#define INVOKE_V_R(api_handle, call_name, in_param0) \
    const auto& vs = fc::json::json::from_string(body).as<fc::variants>(); \
    api_handle->call_name(vs.at(0).as<in_param0>()); \
    eosio::detail::vim_empty result;

#define INVOKE_V_R_R_R_R_R(api_handle, call_name, in_param0, in_param1, in_param2, in_param3, in_param4) \
    const auto& vs = fc::json::json::from_string(body).as<fc::variants>(); \
    api_handle->call_name(vs.at(0).as<in_param0>(), vs.at(1).as<in_param1>(), vs.at(2).as<in_param2>(), vs.at(3).as<in_param3>(), vs.at(4).as<in_param4>()); \
    eosio::detail::vim_empty result;

struct vim_plugin_impl {
    void push_transaction(signed_transaction& trx) { try {
       chain_plugin& cp = app().get_plugin<chain_plugin>();
       cp.accept_transaction( packed_transaction(trx), nullptr );
    } FC_CAPTURE_AND_RETHROW( (transaction_header(trx)) ) }

    void create_account(const std::string& acc_name) {
        fc::crypto::private_key producer_priv_key(std::string("5KQwrPbwdL6PhXujxW37FSSQZ1JiwsST4cqQzDeyXtP79zkvFD3"));
        auto owner_priv_key = private_key_type::generate();
        auto active_priv_key = private_key_type::generate();

        auto owner_auth = eosio::chain::authority{1, {{owner_priv_key.get_public_key(), 1}}, {}};
        auto active_auth = eosio::chain::authority{1, {{active_priv_key.get_public_key(), 1}}, {}};

        controller& chain_controller = app().get_plugin<chain_plugin>().chain();
        auto chainid = app().get_plugin<chain_plugin>().get_chain_id();

        signed_transaction trx;
        trx.actions.emplace_back(vector<chain::permission_level>{{account_name("eosio"), "active"}},
                                 newaccount{account_name("eosio"), name(acc_name), owner_auth, active_auth});
        trx.expiration = chain_controller.head_block_time() + fc::seconds(30);
        trx.set_reference_block(chain_controller.head_block_id());
        trx.sign(producer_priv_key, chainid);
        push_transaction(trx);
    }

    void create_post(std::string acc_name, std::string url, std::string hash, std::string param_first, std::string param_second) {

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
        CALL(vim, my, create_account, INVOKE_V_R(my, create_account, std::string), 200),
        CALL(vim, my, create_post, INVOKE_V_R_R_R_R_R(my, create_post, std::string, std::string, std::string, std::string, std::string), 200)
    });
}

void vim_plugin::plugin_shutdown() {

}

}
