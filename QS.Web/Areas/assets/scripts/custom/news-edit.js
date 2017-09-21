var FormEditable = function () {

    var initEditables = function () {

        //新闻标题
        $('#newstitle').editable({
            type: 'text',
            name: 'newsTitle',
            url: '/Admin/NewsManage/Edit',
            title: '修改新闻标题',
            success: function (settings) {
                $('#console').val(settings);
            },
            error: function (settings) {
                $('#console').text(settings);
            },
            ajaxOptions: {
                type: 'post'
            }
        });

        //是否置顶
        $('#top').editable({
            inputclass: 'form-control',
            source: [{
                    value: 1,
                    text: '置顶'
                }, {
                    value: 0,
                    text: '默认'
                }
            ],
            display: function (value, sourceData) {
                var colors = {
                    "": "gray",
                    0: "blue",
                    1: "red"
                },
                    elem = $.grep(sourceData, function (o) {
                        return o.value == value;
                    });

                if (elem.length) {
                    $(this).text(elem[0].text).css("color", colors[value]);
                } else {
                    $(this).empty();
                }
            },
            success: function (settings) {
                $('#console').val($('#console').val() + '\n' + settings);
            }
        });

        $('#category').editable({
            source: [{
                value: '热点聚焦',
                text: '热点聚焦'
            }, {
                value: '校园动态',
                text: '校园动态'
            }, {
                value: '求索活动',
                text: '求索活动'
            }
            ],
            showbuttons: false,
            success: function (settings) {
            $('#console').val($('#console').val() + '\n' + settings);
        }

        });

        //标签
        $('#tags').editable({
            url: '/Admin/NewsManage/Edit2',
            limit: 3,
            source: '/Admin/NewsManage/BindNewsTags',
            params: function (params) {
                return JSON.stringify(params);
            }
        });

        $('#tags').on('shown', function (e, reason) {
            App.initUniform();
        });

        /*
        $('#tags').editable({
            inputclass: 'form-control input-medium',
            url: '/Admin/NewsManage/Edit',
            select2: {
                placeholder: '点击选择标签',
                allowClear: true,
                multiple: true,
                id: function(item) {
                    return item.value;
                },
                ajax: {
                    url: '/Admin/NewsManage/BindNewsTags',
                    dataType: 'json',
                    data: function(term, page) {
                        return { query: term };
                    },
                    results: function(data, page) {
                        return { results: data };
                    }
                },
                formatResult: function(item) {
                    return item.text;
                },
                formatSelection: function(item) {
                    return item.text;
                },
                initSelection: function(element, callback) {
                    return $.get('/Admin/NewsManage/GetTagById', {query: element.val()}, function(data) {
                        callback(data);
                    });
                }
            },
            success: function (settings) {
                $('#console').val($('#console').val() + '\n' + settings);
            }
        });

        */
    }

    return {
        //main function to initiate the module
        init: function () {

            // init editable elements
            initEditables();
            
            // init editable toggler
            $('#enable').click(function () {
                $('#user .editable').editable('toggleDisabled');
                if (document.getElementById("editSwitch").disabled == true) {
                    document.getElementById("editSwitch").disabled = false;
                } else {
                    document.getElementById("editSwitch").disabled = true;
                }
            });

            $('#delete').click(function() {
                bootbox.confirm("你确定删除这篇文章？ [此操作不可恢复]", function(result) {
                    if (result) {
                        $.ajax({
                            type: 'post',
                            data: { id: $('#nid').val() },
                            url: '/Admin/NewsManage/Delete',
                            success: function(evt) {
                                if (evt == "删除成功") {
                                    bootbox.alert("删除成功", function() {
                                        window.location = "/Admin/NewsManage/Index";
                                    });
                                }
                            },
                            error: function(scrip) {
                                bootbox.alert("操作失败，请重新操作");
                            }
                        });
                    }
                });
            });

            // handle editable elements on hidden event fired
            $('#user .editable').on('hidden', function (e, reason) {
                if (reason === 'save' || reason === 'nochange') {
                    var $next = $(this).closest('tr').next().find('.editable');
                    if ($('#autoopen').is(':checked')) {
                        setTimeout(function () {
                            $next.editable('show');
                        }, 300);
                    } else {
                        $next.focus();
                    }
                }
            });
        }
    };

}();