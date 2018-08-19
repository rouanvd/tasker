module Webfu.DOM.Lang where 

import Prelude

data Lang 
  = EnZA
  | EnUS

instance showLang :: Show Lang where 
  show :: Lang -> String 
  show EnZA = "en-ZA"
  show EnUS = "en-US"




