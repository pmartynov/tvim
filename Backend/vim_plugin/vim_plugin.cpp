#include <eosio/vim_plugin/vim_plugin.hpp>

namespace eosio {
   static appbase::abstract_plugin& _vim_plugin = app().register_plugin<vim_plugin>();

class vim_plugin_impl {
   public:
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
   if(options.count("option-name")) {

   }
}

void vim_plugin::plugin_startup() {

}

void vim_plugin::plugin_shutdown() {

}

}
