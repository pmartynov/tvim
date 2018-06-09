#pragma once
#include <appbase/application.hpp>
#include <eosio/http_plugin/http_plugin.hpp>

namespace eosio {

using namespace appbase;

class vim_plugin : public appbase::plugin<vim_plugin> {
public:
   vim_plugin();
   virtual ~vim_plugin();
 
   APPBASE_PLUGIN_REQUIRES()
   virtual void set_program_options(options_description&, options_description& cfg) override;
 
   void plugin_initialize(const variables_map& options);
   void plugin_startup();
   void plugin_shutdown();

private:
   std::unique_ptr<class vim_plugin_impl> my;
};

}
