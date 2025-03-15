Code không mở rộng được, các class phụ thuộc vào nhau
Nên sử dụng phiên bản Unity version mới nhất
Cấu trúc lại code, cách đặt tên , sử dụng namespace ( https://www.notion.so/Clean-Code-19d5dad7b61f8073b82dca4955cf974f )
Áp dụng mô hình MVP để quản lý cấu trúc UI ( ko nên để UI trên scene và phải load động)
Dùng Addressable
Dùng Pooling để spawn các object trong game, các item
Dùng TextMeshProUI
Áp dụng Dependency Inject để cấu trúc lại mã code
Nên tổ chức lại các manager ( Audio, Pool, Load.....)
Tách biệt class vs enum để không bị phụ thuộc
Dùng các package ngoài để tối ưu hiệu năng hơn , với game này nên dùng leantouch
https://www.notion.so/Unity-Optimization-19d5dad7b61f80a98c21d5558268f069 nên optimize những cái cơ bản
Cấu trúc code cũ khó mở rộng và kiểm thử tối ưu
Code project trên ko áp dụng được design partent nào + nên áp dụng nguyên tắc Solid https://www.notion.so/SOLID-78c48f9eca5747d883870350a3170526