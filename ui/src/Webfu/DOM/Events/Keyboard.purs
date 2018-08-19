module Webfu.DOM.Events.Keyboard where

import Prelude
import Webfu.Interop
import Webfu.DOM.Events


char :: KeyboardEvent -> String
char = (readString "char") <<< toJsObject

code :: KeyboardEvent -> String
code = (readString "code") <<< toJsObject

key :: KeyboardEvent -> String
key = (readString "key") <<< toJsObject

ctrlKey :: KeyboardEvent -> Boolean
ctrlKey = (readBoolean "ctrlKey") <<< toJsObject

shiftKey :: KeyboardEvent -> Boolean
shiftKey = (readBoolean "shiftKey") <<< toJsObject

altKey :: KeyboardEvent -> Boolean
altKey = (readBoolean "altKey") <<< toJsObject

metaKey :: KeyboardEvent -> Boolean
metaKey = (readBoolean "metaKey") <<< toJsObject

isComposing :: KeyboardEvent -> Boolean
isComposing = (readBoolean "isComposing") <<< toJsObject

repeat :: KeyboardEvent -> Boolean
repeat = (readBoolean "repeat") <<< toJsObject
