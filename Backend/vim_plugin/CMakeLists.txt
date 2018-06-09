file(GLOB HEADERS "include/eosio/vim_plugin/*.hpp")
add_library( vim_plugin
             vim_plugin.cpp
             ${HEADERS} )

target_link_libraries( vim_plugin appbase fc )
target_include_directories( vim_plugin PUBLIC "${CMAKE_CURRENT_SOURCE_DIR}/include" )
