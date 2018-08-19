'use strict';

exports.responseOkImpl = function(response) {
  return response.ok;
};


exports.responseStatusImpl = function(response) {
  return response.status;
};


exports.responseStatusTextImpl = function(response) {
  var statusText = response.statusText;
  if (statusText == null)
    return "";

  return statusText;
};


exports.responseBodyAsTextImpl = function(response) {
  var responseText = response.text();
  if (responseText == null)
    return "";

  return responseText;
};


exports.responseBodyAsJsonImpl = function(response) {
  var responseJson = response.json();
  if (responseJson == null)
    return {};

  return responseJson;
};


exports.mkHeadersImpl = function() {
  var newHeaders = new Headers();
  return newHeaders;
};


exports.headersAppendImpl = function(key, value, headers) {
  headers.append( key, value );
  return headers;
};


exports.win_fetch_foreign = function (url) {
  return function (options) {
    return function (w) {

      return function () { // Eff wrapper
        var promise = w.fetch( url, options );
        return promise;
      };

    };
  };
};

