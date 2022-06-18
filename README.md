# FNode
A Nodebased File Management System

# Node ideas:
- Directory Name
- Split Text at... (variable Output Size?? implement text / general Array Sockets???)
- Int to Filesize (KB, MB, GB...)
- random Text (from given List)
- random Number (min, max)
- Delete File (option: to Recycle bin / for good)
- convertion
    - Images: Magick.Net
- zip Files
- Read Text Files
    Options: Whole Text, Only Line X, plit at string...
- Image Nodes:
    - Crop (pixels / percent, t l b r)
    - Resize (pixels / percent, option: Keep Aspect)

### Wild Ideas:
- Parse JSON
- Rest API Request
- Create PDFs

# Text Selection Workflow Idea
- add selected property to each character
    - this could be a separate array of ints
    - characters at these positions will be treated as not-selected
- by default, select everything (empty array)
- perform text operation sonly on selected characters

# General Ideas
- Add Possibility to preview critical File Operaions (Delete, Move, Rename...)

### Auto Slot Conversions
o = todo  
x = done - conversion possible  
d = working by default  
e = done - conversion nonsensical so throwing error and returning null

| / | file | string | bool |int | float | date | list |
|-|-|-|-|-|-|-|-|
| file | - | x | o (does exist) | o | o | o | o |
| String | o | - | o (is not empty) | o | o | o | x |
| bool | o | x | - | o | o | o | x |
| int | o | x | x | - | o | o | x |
| float | o | x | x | o | - | o | x |
| date | o | x | o | o | o | - | o |
| null | o | x | o | o | o | o | o | o |