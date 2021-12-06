const MESSAGE = {
    'insertSucess': {
        'title': 'Thành Công',
        'content': 'Thêm Thành Công',
    },
    'insertError': {
        'title': 'Lỗi',
        'content': 'Thêm Thất Bại'
    },
    'updatetSucess': {
        'title': 'Thành Công',
        'content': 'Cập Nhật Thành Công',
    },
    'updateError': {
        'title': 'Lỗi',
        'content': 'Cập Nhật Thất Bại'
    },
    'evaluationError': {
        'title': 'Lỗi',
        'content': 'Dữ Liệu Không Hợp Lệ'
    },
    'evaluationSuccess': {
        'title': 'Thành Công',
        'content': 'Đánh Giá Thành Công'
    },
    soundSuccess: function () {
        sound("/Assets/mp3/smallbox.mp3");
    },
    soundError: function () {
        sound("/Assets/mp3/error.mp3");
    }
}