var watson = require('watson-developer-cloud');
var fs = require("fs");
var retrieve_and_rank = watson.retrieve_and_rank({
  username: '8a905ee6-6fab-4a28-80cd-8ea5ec1e8fd5',
  password: 'urgjXadoRbR1',
  version: 'v1'
});
//"ranker_id":"c852c8x19-rank-3488"
Mycluster_id = 'sc75ab4474_e7c4_48ae_adec_07dc35726001';
console.log("hello");

retrieve_and_rank.listClusters({},
  function (err, response) {
    if (err)
      console.log('error:', err);
    else
      console.log(JSON.stringify(response, null, 2));
});
//
// retrieve_and_rank.listConfigs({
//   cluster_id: Mycluster_id
// },
//   function (err, response) {
//     if (err)
//       console.log('error:', err);
//     else
//       console.log(JSON.stringify(response, null, 2));
// });
//
// var myparams = {
//   cluster_id: Mycluster_id,
//   config_name: 'Sahil_config',
//   config_zip_path: 'cranfield-solr-config.zip'
// };
//
// retrieve_and_rank.uploadConfig(myparams,
//   function (err, response) {
//     if (err)
//       console.log('error:', err);
//     else
//       console.log(JSON.stringify(response, null, 2));
// });

// var params = {
//   cluster_id: Mycluster_id,
//   config_name: 'Sahil_config',
//   collection_name: 'sahil_collection',
//   wt: 'json'
// };
//
// retrieve_and_rank.createCollection(params,
//   function (err, response) {
//     if (err)
//       console.log('error:', err);
//     else
//       console.log(JSON.stringify(response, null, 2));
// });
