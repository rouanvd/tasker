module Webfu.DOM.Events.Mouse where

import Prelude
import Webfu.DOM.Events
import Webfu.Interop


detail :: MouseEvent -> Number
detail = (readNumber "detail") <<< toJsObject

screenX :: MouseEvent -> Int
screenX = (readInt "screenX") <<< toJsObject

screenY :: MouseEvent -> Int
screenY = (readInt "screenY") <<< toJsObject

clientX :: MouseEvent -> Int
clientX = (readInt "clientX") <<< toJsObject

clientY :: MouseEvent -> Int
clientY = (readInt "clientY") <<< toJsObject

button :: MouseEvent -> Int
button = (readInt "button") <<< toJsObject

buttons :: MouseEvent -> Int
buttons = (readInt "buttons") <<< toJsObject

ctrlKey :: MouseEvent -> Boolean
ctrlKey = (readBoolean "ctrlKey") <<< toJsObject

shiftKey :: MouseEvent -> Boolean
shiftKey = (readBoolean "shiftKey") <<< toJsObject

altKey :: MouseEvent -> Boolean
altKey = (readBoolean "altKey") <<< toJsObject

metaKey :: MouseEvent -> Boolean
metaKey = (readBoolean "metaKey") <<< toJsObject
