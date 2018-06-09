#include <eosio/vim_plugin/vim_plugin.hpp>

#include <eosio/chain_plugin/chain_plugin.hpp>
#include <eosio/chain/wast_to_wasm.hpp>
#include <eosio/utilities/key_conversion.hpp>

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
        //fc::crypto::private_key producer_priv_key(std::string("5KQwrPbwdL6PhXujxW37FSSQZ1JiwsST4cqQzDeyXtP79zkvFD3"));
        controller& chain_controller = app().get_plugin<chain_plugin>().chain();
//        auto chainid = app().get_plugin<chain_plugin>().get_chain_id();
        abi_serializer serializer = fc::json::from_string(vim_contract_abi).as<abi_def>();

        action act;
        act.account = N(eosio);
        act.name = N(createacc);
        act.authorization = vector<permission_level>{{name("newacc"), config::active_name}};
        act.data = serializer.variant_to_binary(
                    "createacc", fc::json::from_file(
                        fc::format_string(
                            "{\"account\":\"${acc_name}\"}",
                            fc::mutable_variant_object()("acc_name", acc_name))));

        signed_transaction trx;
        trx.actions.push_back(act);
        trx.expiration = chain_controller.head_block_time() + fc::seconds(30);
        trx.set_reference_block(chain_controller.head_block_id());
//        trx.sign(producer_priv_key, chainid);
        push_transaction(trx);
    }

    void create_post(const std::string& acc_name, const std::string& url, const std::string& hash, const std::string& requesting_acc_name, const std::string& key) {
        controller& chain_controller = app().get_plugin<chain_plugin>().chain();
        abi_serializer serializer = fc::json::from_string(vim_contract_abi).as<abi_def>();

        action act;
        act.account = N(eosio); //
        act.name = N(createpost);
        act.authorization = vector<permission_level>{{name(requesting_acc_name), config::active_name}}; //
        act.data = serializer.variant_to_binary(
                    "createpost", fc::json::from_file(
                        fc::format_string(
                            "{\"creator\":\"${acc_name}\", \"url_photo\":\"${url}\", \"hash_photo\":\"${hash}\"}",
                            fc::mutable_variant_object()("acc_name", acc_name)("url", url)("hash", hash))));

        signed_transaction trx;
        trx.actions.push_back(act);
        trx.expiration = chain_controller.head_block_time() + fc::seconds(30);
        trx.set_reference_block(chain_controller.head_block_id());
        push_transaction(trx);
    }

    void transfer(const std::string& from, const std::string& to, const std::string& amount, const std::string& url, const std::string& hash) {
        controller& chain_controller = app().get_plugin<chain_plugin>().chain();
        abi_serializer serializer = fc::json::from_string(vim_contract_abi).as<abi_def>();

        action act;
        act.account = N(eosio); //
        act.name = N(transfer);
        act.authorization = vector<permission_level>{{name("newacc"), config::active_name}}; //
        act.data = serializer.variant_to_binary(
                    "transfer", fc::json::from_file(
                        fc::format_string(
                            "{\"from\":\"${from}\", \"to\":\"${to}\", \"amount\":\"${amount}\", \"url_photo\":\"${url}\", \"hash_photo\":\"${hash}\"}",
                            fc::mutable_variant_object()("from", from)("to", to)("amount", amount)("url", url)("hash", hash))));

        signed_transaction trx;
        trx.actions.push_back(act);
        trx.expiration = chain_controller.head_block_time() + fc::seconds(30);
        trx.set_reference_block(chain_controller.head_block_id());
        push_transaction(trx);
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
        CALL(vim, my, transfer, INVOKE_V_R_R_R_R_R(my, transfer, std::string, std::string, std::string, std::string, std::string), 200),
        CALL(vim, my, create_post, INVOKE_V_R_R_R_R_R(my, create_post, std::string, std::string, std::string, std::string, std::string), 200)
    });
}

void vim_plugin::plugin_shutdown() {

}

}
